
// Database Schema
public static class DatabaseSchema
{
    public static string UsersTable = @"CREATE TABLE IF NOT EXISTS Users(
        id INT AUTO_INCREMENT PRIMARY KEY,
        verified BOOLEAN,
        photo VARCHAR(255) DEFAULT NULL,
        privacy ENUM('public', 'private', 'friends'),
        username VARCHAR(60) UNIQUE,
        first_name VARCHAR(60),
        last_name VARCHAR(60),
        password VARCHAR(255),
        email VARCHAR(60),
        created_at DATETIME
    )";

    public static string PostsTable = @"CREATE TABLE IF NOT EXISTS Posts(
        id INT AUTO_INCREMENT PRIMARY KEY,
        photo VARCHAR(255) DEFAULT NULL,
        privacy ENUM('public', 'private', 'friends'),
        user_id INT,
        caption VARCHAR(1024),
        created_at DATETIME,
        FOREIGN KEY (user_id) REFERENCES Users(id)
    )";

    public static string StoriesTable = @"CREATE TABLE IF NOT EXISTS Stories(
        id INT AUTO_INCREMENT PRIMARY KEY,
        user_id INT,
        photo VARCHAR(255) DEFAULT NULL,
        content VARCHAR(255),
        privacy ENUM('public', 'private', 'friends'),
        created_at DATETIME,
        FOREIGN KEY (user_id) REFERENCES Users(id)
    )";

    public static string ReactionsPostsTable = @"CREATE TABLE IF NOT EXISTS ReactionsPosts(
        user_id INT,
        post_id INT,
        type BOOLEAN,
        PRIMARY KEY (user_id, post_id),
        FOREIGN KEY (user_id) REFERENCES Users(id),
        FOREIGN KEY (post_id) REFERENCES Posts(id)
    )";
    public static string ReactionsCommentsTable = @"CREATE TABLE IF NOT EXISTS ReactionsComments(
        user_id INT,
        comment_id INT,
        type BOOLEAN,
        PRIMARY KEY (user_id, comment_id),
        FOREIGN KEY (user_id) REFERENCES Users(id),
        FOREIGN KEY (comment_id) REFERENCES Comments(id)
    )";

    public static string ReactionsRepliesTable = @"CREATE TABLE IF NOT EXISTS ReactionsReplies(
        user_id INT,
        reply_id INT,
        type BOOLEAN,
        PRIMARY KEY (user_id, reply_id),
        FOREIGN KEY (user_id) REFERENCES Users(id),
        FOREIGN KEY (reply_id) REFERENCES Replies(id)
    )";

    public static string CommentsTable = @"CREATE TABLE IF NOT EXISTS Comments(
        id INT AUTO_INCREMENT PRIMARY KEY,
        user_id INT,
        post_id INT,
        photo VARCHAR(255) DEFAULT NULL,
        content VARCHAR(1024),
        created_at DATETIME,
        FOREIGN KEY (user_id) REFERENCES Users(id),
        FOREIGN KEY (post_id) REFERENCES Posts(id)
    )";

    public static string RepliesTable = @"CREATE TABLE IF NOT EXISTS Replies(
        id INT AUTO_INCREMENT PRIMARY KEY,
        user_id INT,
        post_id INT,
        comment_id INT,
        photo VARCHAR(255) DEFAULT NULL,
        content VARCHAR(1024),
        created_at DATETIME,
        FOREIGN KEY (comment_id) REFERENCES Comments(id),
        FOREIGN KEY (user_id) REFERENCES Users(id),
        FOREIGN KEY (post_id) REFERENCES Posts(id)
    )";

    public static string GroupsTable = @"CREATE TABLE IF NOT EXISTS Groups_(
        id INT AUTO_INCREMENT PRIMARY KEY,
        photo VARCHAR(255) DEFAULT NULL,
        privacy ENUM('public', 'private', 'friends'),
        description VARCHAR(1024),
        name VARCHAR(60) UNIQUE,
        created_at DATETIME
    )";

    public static string GroupAdminTable = @"CREATE TABLE IF NOT EXISTS GroupAdmin(
        group_id INT,
        admin_id INT,
        PRIMARY KEY(group_id, admin_id),
        FOREIGN KEY (group_id) REFERENCES Groups_(id),
        FOREIGN KEY (admin_id) REFERENCES Users(id)
    )";

    public static string GroupUserTable = @"CREATE TABLE IF NOT EXISTS GroupUser(
        group_id INT,
        user_id INT,
        PRIMARY KEY(group_id, user_id),
        FOREIGN KEY (group_id) REFERENCES Groups_(id),
        FOREIGN KEY (user_id) REFERENCES Users(id)
    )";
}
