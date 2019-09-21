using System;

namespace TddXt.SimpleNlp
{
  public class ConflictingEntityException : Exception
  {
    public ConflictingEntityException(EntityName attemptedEntityName, EntityName existingEntityName, EntityForm conflictingForm)
      : base($"The phrase '{conflictingForm}' cannot be added for the entity '{attemptedEntityName}', " +
             $"because it is already present for entity '{existingEntityName}', which would be matched earlier")
    {
      
    }
  }
}