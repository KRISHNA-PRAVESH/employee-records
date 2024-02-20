using NHibernate.Mapping.Attributes;

public class Employee{


    public virtual int Id { get; set; }

    public virtual string? name { get; set; }

    public virtual string? dob { get; set; }

    public virtual int age { get; set; }

    public virtual string? gender { get; set; }


    public virtual string? mobile { get; set; }
  

}