# EmployeeDirectory

Клиент-серверное приложение - прототип системы для справочника сотрудников.

Клиентская часть - WPF <br/>
Серверная часть - ASP.NET Core Web API

Функционал:
- Просмотр списка сотрудников (постраничный) и их телефонов
- Поиск сотрудников по ФИО
- Добавление новых сотрудников и изменение их данных
- Удаление сотрудников
- Добавление, изменение и удаление телефонов сотрудников

Аутентификация не реализована.

Приложение использует СУБД MSSQL. Чтобы подготовить базу данных для работы, выполните на ней все запросы из файла init.sql.

Строка подключения к базе данных настраивается в файле WebApi/Properties/configs.json.
