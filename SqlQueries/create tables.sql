
drop table dbo.Employees
GO
CREATE TABLE dbo.Employees
(
	id INT NOT NULL IDENTITY,
	login NVARCHAR(max) NOT NULL,
	first_name NVARCHAR(max) NOT NULL,
	second_name NVARCHAR(max),
	middle_name NVARCHAR(max),
	birthday DATE,
	hashsum NVARCHAR(max) NOT NULL,
	PRIMARY KEY	(id)
)

drop table dbo.Phones
GO
CREATE TABLE dbo.Phones
(
	phone_value VARCHAR(11) NOT NULL,
	id_employee INT REFERENCES Employees (id) ON DELETE CASCADE,
	PRIMARY KEY	(phone_value)
)

CREATE INDEX index_id_employee ON Phones (id_employee)