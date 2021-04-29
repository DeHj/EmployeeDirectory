USE EmployeesDirectory

/*
-- result = 0 - success
-- result = 1 - employee with id = @employee_id not exist
DROP PROCEDURE delete_employee
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
*/

/*
-- result = 0 - success
-- result = 1 - employee with id = @employee_id not exist
DROP PROCEDURE change_user
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
*/


-- result = 0 - success
-- result = 2627 - phone number @phone_value already exist
-- result = 547 - user with id = @id_employee does not exist
DROP PROCEDURE add_phone
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


/*
-- result = 0 - success
-- result = 1 - phone number @phone_number does not exist
DROP PROCEDURE delete_phone
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
*/