# Как использовать:
  - Скачать .NET SDK 8.0 [ссылка](https://dotnet.microsoft.com/ru-ru/download)
  - Установить PostgreSQL [ссылка](https://www.postgresql.org/download/)

---

1. В терминале IDE выполнить
  ```bash
  git clone https://github.com/SeeYoo64/OntuPhdApi.git  
  cd OntuPhdApi  
  ```

2. В PostgreSQL
  - Создать базу данных
    ```sql
    CREATE DATABASE ontu_phd;  
    ```
  - Создать таблицу
    ```sql
    CREATE TABLE Doctors (  
    Id SERIAL PRIMARY KEY,  
    Name VARCHAR(100) NOT NULL,  
    Degree VARCHAR(50) NOT NULL
    );  
    INSERT INTO Doctors (Name, Degree) VALUES  
    ('Імя1 1 ', 'Завідувач1 відділу1'),  
    ('Імя2 2 ', 'Завідувач2 відділу2');  
    ```

 3. Настрой конфигурацию:
    - в файле appsettings.json изменить **_Username_** и **_password_** на свои данные для PostgreSQL

 4. Запусти проект:
    - в терминале в папке проекта:
      ```bash
      dotnet run
      ```
  Если всё ок, то увидишь сообщение вроде "Now listening on: http://localhost:5124" <br>

  Теперь в браузере можешь перейти на страницу + /swagger для удобного тестирования API. <br>
  Так-же в Postman`e можно заходить на http://localhost:5124/api/doctors
  



















