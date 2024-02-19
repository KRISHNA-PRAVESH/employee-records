
    drop table if exists Employees

    create table Employees (
        id INTEGER not null,
       name VARCHAR(255),
       dob VARCHAR(255),
       age INTEGER,
       gender VARCHAR(255),
       mobile VARCHAR(255),
       primary key (id)
    )
