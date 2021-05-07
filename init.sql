
-- create tables:

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

GO
CREATE TABLE dbo.Phones
(
	phone_number VARCHAR(11) NOT NULL,
	id_employee INT REFERENCES Employees (id) ON DELETE CASCADE,
	PRIMARY KEY	(phone_number)
)

CREATE INDEX index_id_employee ON Phones (id_employee)


-- create stored procedures for getting employees data:

GO
CREATE PROCEDURE get_all_employees
	@from int,
	@page_size int
AS
BEGIN
	SELECT login, first_name, second_name, middle_name, birthday, id from Employees
	ORDER BY id
	OFFSET @from ROW FETCH NEXT @page_size ROW ONLY
END



GO
CREATE PROCEDURE get_employees_by_name
	@first_name NVARCHAR(max) = '%',
	@second_name NVARCHAR(max) = '%',
	@middle_name NVARCHAR(max) = '%',
	@from int,
	@page_size int
AS
BEGIN
	SELECT login, first_name, second_name, middle_name, birthday, id from Employees
	WHERE
		first_name LIKE @first_name
		AND ((second_name IS NULL AND @second_name = '%') OR second_name LIKE @second_name)
		AND ((middle_name IS NULL AND @middle_name = '%') OR middle_name LIKE @middle_name)
	ORDER BY id
	OFFSET @from ROW FETCH NEXT @page_size ROW ONLY
END



GO
CREATE PROCEDURE get_phones_by_id
	@id_employee int
AS
BEGIN
	SELECT phone_number, id_employee from Phones
	WHERE id_employee = @id_employee
END



GO
CREATE PROCEDURE get_employee_by_id
	@id_employee int
AS
BEGIN
	SELECT login, first_name, second_name, middle_name, birthday, id from Employees
	WHERE id = @id_employee
END;

-- create stored procedures for change employees data:

-- result = 0 - success
-- result = 1 - user with such login already exist
GO
CREATE PROCEDURE add_user
	@login NVARCHAR(max),
	@first_name NVARCHAR(max),
	@second_name NVARCHAR(max) = NULL,
	@middle_name NVARCHAR(max) = NULL,
	@hashsum NVARCHAR(max),
	@employee_id INT OUTPUT,
	@result INT OUTPUT
AS
BEGIN
	IF (SELECT count(*) FROM Employees WHERE Employees.login = @login) = 0
	BEGIN
		INSERT INTO Employees (login, first_name, second_name, middle_name, hashsum) VALUES
		(@login, @first_name, @second_name, @middle_name, @hashsum);
		SELECT @result = 0
		SELECT @employee_id = (SELECT id FROM Employees WHERE login = @login)
		RETURN
	END
	ELSE
	BEGIN
		SELECT @result = 1
		RETURN
	END
END



-- result = 0 - success
-- result = 1 - employee with id = @employee_id not exist
GO
CREATE PROCEDURE delete_employee
	@employee_id NVARCHAR(max),
	@result INT OUTPUT
AS
BEGIN
	IF (SELECT COUNT(*) FROM Employees WHERE Employees.id = @employee_id) = 0
	BEGIN
		SELECT @result = 1
	END
	ELSE
	BEGIN
		SELECT @result = 0
		DELETE FROM Employees WHERE id = @employee_id
	END
END



-- result = 0 - success
-- result = 1 - employee with id = @employee_id not exist
GO
CREATE PROCEDURE change_user
	@employee_id int,
	@first_name NVARCHAR(max) = NULL,
	@second_name NVARCHAR(max) = NULL,
	@middle_name NVARCHAR(max) = NULL,
	@birthday DATE = NULL,
	@new_hashsum NVARCHAR(max) = NULL,
	@result INT OUTPUT
AS
BEGIN
	IF (SELECT COUNT(*) FROM Employees WHERE Employees.id = @employee_id) = 1
	BEGIN
		SELECT @result = 0
		IF (@first_name IS NOT NULL)
		BEGIN
			UPDATE employees
			SET first_name = @first_name
			WHERE Employees.id = @employee_id;
		END
		IF (@second_name IS NOT NULL)
		BEGIN
			UPDATE employees
			SET second_name = @second_name
			WHERE Employees.id = @employee_id;
		END
		IF (@middle_name IS NOT NULL)
		BEGIN
			UPDATE employees
			SET middle_name = @middle_name
			WHERE Employees.id = @employee_id;
		END
		IF (@birthday IS NOT NULL)
		BEGIN
			UPDATE employees
			SET birthday = @birthday
			WHERE Employees.id = @employee_id;
		END
		IF (@new_hashsum IS NOT NULL)
		BEGIN
			UPDATE employees
			SET hashsum = @new_hashsum
			WHERE Employees.id = @employee_id;
		END
	END
	ELSE
	BEGIN
		SELECT @result = 1
	END
END



-- result = 0 - success
-- result = 2627 - phone number @phone_number already exist
-- result = 547 - user with id = @id_employee does not exist
GO
CREATE PROCEDURE add_phone
	@employee_id INT,
	@phone_number VARCHAR(11),
	@result INT OUTPUT
AS
BEGIN
	BEGIN TRY
		INSERT INTO Phones (phone_number, id_employee) VALUES
		(@phone_number, @employee_id)
		SELECT @result = 0
	END TRY
	BEGIN CATCH
		SELECT @result = ERROR_NUMBER()
	END CATCH
END



-- result = 0 - success
-- result = 1 - phone number @phone_number does not exist
GO
CREATE PROCEDURE delete_phone
	@phone_number VARCHAR(11),
	@result INT OUTPUT
AS
BEGIN
	IF (SELECT COUNT(*) FROM Phones WHERE phone_number = @phone_number) = 1
	BEGIN
		DELETE FROM Phones WHERE phone_number = @phone_number
		SELECT @result = 0
	END
	ELSE
	BEGIN
		SELECT @result = 1
	END
END
