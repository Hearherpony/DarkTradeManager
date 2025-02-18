# DarkTradeManager
#### Пузаков Дмитрий ИСП-6

**DarkTradeManager** – это WPF-приложение для управления торговой базой данных, построенное на MS SQL, C# WPF и Entity Framework.  
Проект включает:
- Создание базы данных с таблицами пользователей, ролей, заказов, товаров и связей.
- Импорт данных из Excel-файлов.
- Интерфейс авторизации с динамической капчей.
- Главный экран с поиском, фильтрацией и отображением списка товаров.
- Дизайн в стиле темной темы.

## Структура проекта

- **DarkTradeManager (WPF Application)**
  - **App.config** – конфигурация приложения (строка подключения).
  - **DarkDbContext.cs** – контекст Entity Framework.
  - **Models:**  
    - `UserEntity.cs`, `UserRole.cs`, `ItemEntity.cs`
  - **Converters:**  
    - `DarkImageConverter.cs` – конвертер для отображения изображений товаров.
  - **Views:**
    - `AuthWindow.xaml` / `AuthWindow.xaml.cs` – окно авторизации.
    - `DashboardWindow.xaml` / `DashboardWindow.xaml.cs` – главное окно с товарами.
- **SQL Скрипты:**  
  - Скрипты для создания базы данных и импорта данных из Excel.
- **Ресурсы:**  
  - Excel-файлы импорта и изображения находятся по пути: `D:/Teh/задание/`