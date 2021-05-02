use EmployeesDirectory

/*
DROP PROCEDURE dbo.get_all_employees;
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



DROP PROCEDURE dbo.get_employees_by_name;
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
END;



DROP PROCEDURE dbo.get_phones_by_id;
GO
CREATE PROCEDURE get_phones_by_id
	@id_employee int
AS
BEGIN
	SELECT phone_number, id_employee from Phones
	WHERE id_employee = @id_employee
END;
*/


GO
CREATE PROCEDURE get_employee_by_id
	@id_employee int
AS
BEGIN
	SELECT login, first_name, second_name, middle_name, birthday, id from Employees
	WHERE id = @id_employee
END;