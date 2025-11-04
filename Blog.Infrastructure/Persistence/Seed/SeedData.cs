using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.ValueObjects;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Persistence.Seed
{
    public class SeedData
    {
        public static async Task SeedDb(BlogDbContext context, CancellationToken ct = default)
        {
            await context.Database.MigrateAsync(ct);

            const string locale = "en";
            var faker = new Faker(locale);
            const int sentenceLimit = 5, wordsLimit = 10;
            var random = new Random();

            #region Seed Users
            if (!await context.Users.AnyAsync(ct))
            {
            var usersToAdd = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                var userName = faker.Internet.UserName().ToLowerInvariant();
                var user = new User
                {
                    UserName = userName,
                    Email = Email.Create($"{userName}@example.com"),
                    Bio = faker.Lorem.Sentence(wordsLimit),
                };
                    usersToAdd.Add(user);
            }

            await context.Users.AddRangeAsync(usersToAdd, ct);
            await context.SaveChangesAsync(ct);
        }
            #endregion

            #region Seed Posts 
                var Authors = await context.Users.AsNoTracking().ToListAsync(ct);

            if (!await context.Posts.AnyAsync(ct))
            {
                var postsToAdd = new List<Post>();

                for (int i = 0; i < 50; i++)
                {
                    var author = faker.PickRandom(Authors); 
                    var post = new Post
                    {
                        UserId = author.Id,
                        Content = faker.Lorem.Paragraph(random.Next(sentenceLimit)) 
                    };
                    postsToAdd.Add(post);
                }

                await context.Posts.AddRangeAsync(postsToAdd, ct);
                await context.SaveChangesAsync(ct);
            }
            #endregion

            #region Seed Comments

                var posts = await context.Posts.AsNoTracking().ToListAsync(ct);
            if (!await context.Comments.AnyAsync(ct))
            {
                var commentsToAdd = new List<Comment>();

                for(int i = 0; i < 100; i++)
                {
                    var post = faker.PickRandom(posts);
                    var author = faker.PickRandom(Authors);

                    commentsToAdd.Add(new Comment
                    {
                        UserId = author.Id,
                        PostId = post.Id,
                        Content = faker.Lorem.Sentence(random.Next(wordsLimit))
                    });
                }

                await context.Comments.AddRangeAsync(commentsToAdd, ct);
                await context.SaveChangesAsync(ct);
            }
            #endregion

            #region Seed Reactions

            if(!await context.Reactions.AnyAsync(ct))
            {

                var comments = await context.Comments.AsNoTracking().ToListAsync(ct);
                var reactionKinds = Enum.GetValues<ReactionKind>();

                var reactionsToAdd = new List<Reaction>(); 
                var reactedUserPosts = new HashSet<(int, int)>();
                var reactedUserComments = new HashSet<(int, int)>();

                for (int i = 0; i < 1000; i++)
                {
                    var author = faker.PickRandom(Authors);
                    var post = faker.PickRandom(posts);
                    var comment = faker.PickRandom(comments);
                    var kind = faker.PickRandom(reactionKinds);
                    bool reactToPost = random.Next(2) == 1;

                    if (reactToPost)
                    {
                        if (reactedUserPosts.Add((author.Id, post.Id)))
                        {
                            reactionsToAdd.Add(new Reaction
                            {
                                UserId = author.Id,
                                PostId = post.Id,
                                Kind = kind
                            });
                        }
                    }
                    else
                    {

                        if (reactedUserComments.Add((author.Id, comment.Id)))
                        {
                            reactionsToAdd.Add(new Reaction
                            {
                                UserId = author.Id,
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

                var followsToAdd = new List<Follow>();
                var existingFollows = new HashSet<(int, int)>();

            for (int i = 0; i < 100; i++)
            {
                var follower = faker.PickRandom(Authors);
                var followee = faker.PickRandom(Authors);

                if(Authors.Count < 2)
                    return;

                if (follower.Id != followee.Id && existingFollows.Add((follower.Id, followee.Id)))
                {
                    followsToAdd.Add(new Follow
                    {
                        FollowerId = follower.Id,
                        FolloweeId = followee.Id
                    });
                }

            }

             if (followsToAdd.Count > 0)
             {
                await context.Follows.AddRangeAsync(followsToAdd, ct);
                await context.SaveChangesAsync(ct);
             }
            #endregion

        }
    }
}
