using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace TddXt.SimpleNlp
{
  public class RecognitionModel
  {
    private readonly InnerRecognitionModel _recognitionModel = new InnerRecognitionModel(); 
    public void AddEntity(string entityName, string value)
    {
      _recognitionModel.AddEntity(EntityName.Value(entityName), value);
    }

    public void AddEntity(string entityName, string value, string[] synonyms)
    {
      _recognitionModel.AddEntity(EntityName.Value(entityName), value, synonyms);
    }

    public void AddIntent(string intentName, IEnumerable<string> entityNames)
    {
      _recognitionModel.AddIntent(intentName, entityNames.Select(EntityName.Value));
    }

    public RecognitionResult Recognize(string text)
    {
      return _recognitionModel.Recognize(text);
    }

  }
}