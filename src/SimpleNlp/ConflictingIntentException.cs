using System;

namespace TddXt.SimpleNlp
{
  public class ConflictingIntentException : Exception
  {
    public ConflictingIntentException(string newIntentName, string existingIntentName)
    : base($"All the phrases matched by '{newIntentName}' will be matched by earlier by '{existingIntentName}'")
    {
      
    }
  }
}