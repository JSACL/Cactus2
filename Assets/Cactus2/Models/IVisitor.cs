public interface IVisitor
{
    //void AddDynamic(dynamic model) => Add(model);
    //void RemoveDynamic(dynamic model) => Add(model);
    void Add(IEntity model);
    void Add(IBullet model);
    void Add(IPlayer model);
    void Add(IFirer model);
    void Remove(IEntity model);
    void Remove(IBullet model);
    void Remove(IPlayer model);
    void Remove(IFirer model);
}