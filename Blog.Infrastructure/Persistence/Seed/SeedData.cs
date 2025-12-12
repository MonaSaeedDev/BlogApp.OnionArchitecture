using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.ValueObjects;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Seed;

public class SeedData
{
    public static async Task SeedDb(BlogDbContext context, CancellationToken ct = default)
    {
        await context.Database.MigrateAsync(ct);

        const string locale = "en";
        var faker = new Faker(locale);
        const int sentenceLimit = 5, wordsLimit = 10;

        #region Seed Users
        var users = new List<User>();
        if (!await context.Users.AnyAsync(ct))
        {
            for (int i = 0; i < 10; i++)
            {
                var userName = faker.Internet.UserName();
                var user = new User
                {
                    UserName = userName,
                    NormalizedUserName = userName.ToUpperInvariant(),
                    Email = Email.Create($"{userName}@example.com"),
                    Bio = faker.Lorem.Sentence(wordsLimit),
                };
                users.Add(user);
            }

            await context.Users.AddRangeAsync(users, ct);
            await context.SaveChangesAsync(ct);
        }
        #endregion

        #region Seed Posts 
        var posts = new List<Post>();
        if (!await context.Posts.AnyAsync(ct))
        {

            for (int i = 0; i < 50; i++)
            {
                var author = faker.PickRandom(users);
                var post = new Post
                {
                    UserId = author.Id,
                    Content = faker.Lorem.Paragraph(Random.Shared.Next(sentenceLimit))
                };
                posts.Add(post);
            }

            await context.Posts.AddRangeAsync(posts, ct);
            await context.SaveChangesAsync(ct);
        }
        #endregion

        #region Seed Comments
        var comments = new List<Comment>();
        if (!await context.Comments.AnyAsync(ct))
        {

            for (int i = 0; i < 100; i++)
            {
                var post = faker.PickRandom(posts);
                var author = faker.PickRandom(users);

                comments.Add(new Comment
                {
                    UserId = author.Id,
                    PostId = post.Id,
                    Content = faker.Lorem.Sentence(Random.Shared.Next(1, wordsLimit + 1))
                });
            }

            await context.Comments.AddRangeAsync(comments, ct);
            await context.SaveChangesAsync(ct);
        }
        #endregion

        #region Seed Reactions
        if (!await context.Reactions.AnyAsync(ct))
        {
            var reactionKinds = Enum.GetValues<ReactionKind>();
            var reactedUserPosts = new HashSet<(int PostId, int UserId)>();
            var reactedUserComments = new HashSet<(int CommentId, int UserId)>();
            var reactionsToAdd = new List<Reaction>();

            for (int i = 0; i < 1000; i++)
            {
                var reactingUser = faker.PickRandom(users);
                var post = faker.PickRandom(posts);
                var comment = faker.PickRandom(comments);
                var kind = faker.PickRandom(reactionKinds);
                bool reactToPost = Random.Shared.Next(2) == 1;

                if (reactToPost)
                {
                    if (reactedUserPosts.Add((post.Id, reactingUser.Id)))
                    {
                        reactionsToAdd.Add(new Reaction
                        {
                            UserId = reactingUser.Id,
                            PostId = post.Id,
                            Kind = kind
                        });
                    }
                }
                else
                {

                    if (reactedUserComments.Add((comment.Id, reactingUser.Id)))
                    {
                        reactionsToAdd.Add(new Reaction
                        {
                            UserId = reactingUser.Id,
                            CommentId = comment.Id,
                            Kind = kind
                        });
                    }
                }
            }

            await context.Reactions.AddRangeAsync(reactionsToAdd, ct);
            await context.SaveChangesAsync(ct);

        }
        #endregion

        #region Seed Follows
        const double density = 0.5;

        var userIds = users.Select(u => u.Id).ToArray();

        var allPairs =
               (from followerId in userIds
                from followeeId in userIds
                where followerId != followeeId
                select new Follow { FollowerId = followerId, FolloweeId = followeeId }
                ).ToList();

        var taken = (int)(allPairs.Count * density);

        var rng = Random.Shared;
        for (int i = allPairs.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (allPairs[i], allPairs[j]) = (allPairs[j], allPairs[i]);
        }

        var followsToAdd = allPairs
            .Take(taken)
            .ToList();

        await context.Follows.AddRangeAsync(followsToAdd, ct);
        await context.SaveChangesAsync(ct);
        #endregion

    }
}
