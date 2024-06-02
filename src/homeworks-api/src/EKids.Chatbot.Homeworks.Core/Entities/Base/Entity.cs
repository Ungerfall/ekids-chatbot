using System;

namespace EKids.Chatbot.Homeworks.Core.Entities.Base;

public abstract class Entity(Guid id) : EntityBase<Guid>(id);
