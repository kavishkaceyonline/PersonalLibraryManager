namespace PersonalLibraryManager.Migrations
{
    using PersonalLibraryManager.Models;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedBooksData : DbMigration
    {
        public override void Up()
        {
            // Replace with your actual user Id (GUID)
            var userId = "882da8fc-24f8-4da9-8e46-ff1a7bb034b9";

            Sql($@"
                INSERT INTO Books (Title, Author, ISBN, Status, Rating, Review, DateAdded, DateCompleted, UserId)
                VALUES 
                ('Clean Code', 'Robert C. Martin', '978-0132350884', {(int)ReadingStatus.Completed}, 5, 
                'Excellent book on writing maintainable code!', '{DateTime.Now.AddDays(-30):yyyy-MM-dd}', '{DateTime.Now.AddDays(-5):yyyy-MM-dd}', '{userId}');

                INSERT INTO Books (Title, Author, Status, Rating, DateAdded, UserId)
                VALUES 
                ('The Pragmatic Programmer', 'Andrew Hunt, David Thomas', {(int)ReadingStatus.Reading}, 4, 
                '{DateTime.Now.AddDays(-10):yyyy-MM-dd}', '{userId}');

                INSERT INTO Books (Title, Author, Status, DateAdded, UserId)
                VALUES 
                ('Design Patterns', 'Gang of Four', {(int)ReadingStatus.ToRead}, 
                '{DateTime.Now.AddDays(-2):yyyy-MM-dd}', '{userId}');
            ");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Books WHERE Title IN ('Clean Code', 'The Pragmatic Programmer', 'Design Patterns')");
        }
    }
}
