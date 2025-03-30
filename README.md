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
  - Восстановить резервную копию
    - ![image](https://github.com/user-attachments/assets/b05c3343-adf8-4ce9-b2d0-29fc553fdac8)
      <br> _Выбери sql файл, под названием - backup_database_
    - ![image](https://github.com/user-attachments/assets/8b129596-e4b5-44c2-b764-39f752ef1210)
      <br> Restore!
 3. Настрой конфигурацию:
    - в файле appsettings.json изменить **_Username_** и **_password_** на свои данные для PostgreSQL

 4. Запусти проект:
    - в терминале в папке проекта:
      ```bash
      dotnet run
      ```
  Если всё ок, то увидишь сообщение вроде "Now listening on: http://localhost:5124" <br>

  Теперь в браузере можешь перейти на страницу + /swagger для удобного тестирования API. <br>
  Так-же в Postman`e можно заходить на http://localhost:5124/api/documents 
  



















