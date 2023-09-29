public interface IVisitor
{
    //void AddDynamic(dynamic model) => Add(model);
    //void RemoveDynamic(dynamic model) => Add(model);
    void Add(IEntity model);
    void Add(IBullet model);
    void Add(ILaser model);
    void Add(IPlayer model);
    void Add(IFirer model);
    void Add(ISpecies1 model);
    void Add(IScene model);
    void Remove(IEntity model);
    void Remove(IBullet model);
    void Remove(ILaser model);
    void Remove(IPlayer model);
    void Remove(IFirer model);
    void Remove(ISpecies1 model);
    void Remove(IScene model);
}