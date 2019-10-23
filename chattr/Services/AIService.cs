using chattr.Models;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Services
{
    /// <summary>
    /// Handles Machine Learning requests
    /// </summary>
    public class AIService
    {
        public Dictionary<string, ITransformer> LoadedModels { get; set; } = new Dictionary<string, ITransformer>();
        /// <summary>
        /// Trains a new machine learning model and returns the file name
        /// </summary>
        /// <returns></returns>
        public string Train(BotConfig config, bool loadModel = true)
        {

            //create dataset from configuration
            var dataset = new List<AIInput>();

            ////create FAQ inputs
            //foreach (var conversation in config.FAQs)
            //{
            //    foreach (var utterance in conversation.StartNode.Utterances)
            //    {
            //        //add utterance
            //        dataset.Add(new AIInput() { Utterance = utterance.Statement.ToLower(), Label = conversation.ID.ToString() });

            //        //add/replace synonyms
            //        foreach (var synonym in conversation.StartNode.Synonyms)
            //        {
            //            if (utterance.Statement.ToLower().Contains(synonym.FAQWord.ToLower()))
            //            {
            //                var replacedPhrase = utterance.Statement.Replace(synonym.FAQWord.ToLower(), synonym.SynonymWord.ToLower());
            //                dataset.Add(new AIInput() { Utterance = replacedPhrase, Label = conversation.ID.ToString() });
            //            }
            //        }

                   
            //    }



            //}

            //create custom conversation inputs
            foreach (var conversation in config.Conversations)
            {
                foreach (var utterance in conversation.StartNode.Utterances)
                {
                    //add utterance
                    dataset.Add(new AIInput() { Utterance = utterance.Statement.ToLower(), Label = conversation.ID.ToString() });

                    //add/replace synonyms
                    foreach (var synonym in conversation.StartNode.Synonyms)
                    {
                        if (utterance.Statement.ToLower().Contains(synonym.FAQWord.ToLower()))
                        {
                            var replacedPhrase = utterance.Statement.Replace(synonym.FAQWord.ToLower(), synonym.SynonymWord.ToLower());
                            dataset.Add(new AIInput() { Utterance = replacedPhrase, Label = conversation.ID.ToString() });
                        }
                    }


                }

            }

            //create ml context for training
            var mlContext = new MLContext(seed: 0);
            var dataView = mlContext.Data.LoadFromEnumerable(dataset);

            //create test train split
            DataOperationsCatalog.TrainTestData dataSplitView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            IDataView trainData = dataSplitView.TrainSet;
            IDataView testData = dataSplitView.TestSet;

            //create pipeline for training
            IEstimator<ITransformer> mlPipeline = BuildTrainingPipeline(mlContext);

            //train model
            ITransformer mlModel = TrainModel(mlContext, trainData, mlPipeline);

            //save model
            var modelFileName = SaveModelFile(mlContext, mlModel, trainData.Schema);
         
            if (loadModel)
            {
                if (LoadedModels.ContainsKey(modelFileName))
                    LoadedModels.Remove(modelFileName);

                LoadedModels.Add(modelFileName, mlModel);
            }

            //return model file name
            return modelFileName;
        }

       
        public AIOutput Predict(BotConfig config, string utterance)
        {
            //attempt to find any exact matches before going to ml
            foreach (var conversation in config.Conversations)
            {
                foreach (var configuredUtterance in conversation.StartNode.Utterances)
                {
                    if (configuredUtterance.Statement.ToLower() == utterance.ToLower())
                    {
                        return new AIOutput() { Prediction = conversation.ID.ToString(), ExactMatch = true, Score = new float[] { 1 } };
                    }    

                }

            }

            MLContext mlContext = new MLContext();
            ITransformer mlModel;
            //load model if not loaded
            if (!LoadedModels.ContainsKey(config.ModelFile))
            {

                if (!System.IO.File.Exists(config.ModelFile))
                {
                    //model was not found!
                    throw new Exception($"Model not found at '{config.ModelFile}'");
                }
                else
                {
                    //add to loaded models for faster future responses
                    var model = mlContext.Model.Load(config.ModelFile, out DataViewSchema inputSchema);
                    LoadedModels.Add(config.ModelFile, model);
                    mlModel = model;
                }

            }
            else
            {
                mlModel = LoadedModels[config.ModelFile];
            }

            var prediction = mlContext.Model.CreatePredictionEngine<AIInput, AIOutput>(mlModel);
            var input = new AIInput() { Utterance = utterance };

            AIOutput result = prediction.Predict(input);
            result.ExactMatch = false;

            return result;
        }

        public void LoadModels()
        {
            throw new NotImplementedException();
        }
        public void UnloadModels()
        {
            throw new NotImplementedException();
        }
        public void ReloadModels()
        {
            throw new NotImplementedException();
        }

        #region MLdotNet
        public static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", "Label")
                                      .Append(mlContext.Transforms.Text.FeaturizeText("Statement_tf", "Statement"))
                                      .Append(mlContext.Transforms.CopyColumns("Features", "Statement_tf"))
                                      .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                                      .AppendCacheCheckpoint(mlContext);

            // Set the training algorithm
            var trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: "Label", numberOfIterations: 10, featureColumnName: "Features"), labelColumnName: "Label")
                                      .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }
        public static ITransformer TrainModel(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            return pipeline.Fit(trainingDataView);
        }

        public static string SaveModelFile(MLContext mlContext, ITransformer mlModel, DataViewSchema modelInputSchema)
        {
           //create guid for model name
           var modelPath = $"Bots\\Models\\{Guid.NewGuid()}.zip";

           //temp override so as not to keep creating files
            modelPath = $"Bots\\Models\\sample.zip";

            //save model
            mlContext.Model.Save(mlModel, modelInputSchema, modelPath);

            //return model path back
            return modelPath;
        }
        #endregion
    }
}
