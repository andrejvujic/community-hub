using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Services;

public class ProviderForumService
{
    private readonly ProviderForumRepository _forumRepository;
    private readonly ForumPostRepository _postRepository;
    private readonly PostReactionRepository _reactionRepository;

    public ProviderForumService(
        ProviderForumRepository forumRepository,
        ForumPostRepository postRepository,
        PostReactionRepository reactionRepository)
    {
        _forumRepository = forumRepository;
        _postRepository = postRepository;
        _reactionRepository = reactionRepository;
    }

    // Vraća forum po ID-u, ili null - ako forum ne postoji.
    public ProviderForum? GetForumById(long forumId)
        => _forumRepository.GetById(forumId);

    // Vraća sve forume vidljive datom serviseru:
    // sve otvorene, i one koje je on kreirao (bez obzira na status).
    public List<ProviderForum> GetVisibleForums(long providerId)
        => _forumRepository.GetVisibleForProvider(providerId);

    // Pretražuje vidljive forume po naslovu ili opisu (ILIKE).
    public List<ProviderForum> SearchForums(string query, long currentProviderId)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Upit za pretragu ne sme biti prazan.", nameof(query));

        return _forumRepository.Search(query.Trim(), currentProviderId);
    }

    // Kreira novi forum. Naslov i opis ne smeju biti prazni (ProviderForum konstruktor to dodatno validira).
    public ProviderForum CreateForum(ServiceProvider creator, string title, string description)
    {
        var forum = new ProviderForum(
            createdBy: creator,
            title: title.Trim(),
            description: description.Trim(),
            isClosed: false,
            createdAt: DateTime.Now
        );

        return _forumRepository.Create(forum);
    }

    // Zatvara forum: Samo autor foruma može zatvoriti svoj forum.
    public void CloseForum(ProviderForum forum, long requestingProviderId)
    {
        if (forum.CreatedBy.Id != requestingProviderId)
            throw new UnauthorizedAccessException(
                "Samo autor foruma može zatvoriti forum.");

        if (forum.IsClosed)
            throw new InvalidOperationException("Forum je već zatvoren.");

        _forumRepository.Close(forum.Id);
    }

    // Vraća sve komentare datog foruma, sortirane hronološki.
    public List<ForumPost> GetComments(long forumId)
        => _postRepository.GetByForumId(forumId);

    // Dodaje komentar na forum. Forum mora biti otvoren.
    // Svaki serviser može komentarisati bilo koji otvoren forum.
    public ForumPost AddComment(ProviderForum forum, ServiceProvider author, string content)
    {
        if (forum.IsClosed)
            throw new InvalidOperationException(
                "Nije moguće komentarisati zatvoren forum.");

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(
                "Sadržaj komentara ne sme biti prazan.", nameof(content));

        var post = new ForumPost(
            forumId: forum.Id,
            author: author,
            content: content.Trim(),
            postedAt: DateTime.Now
        );

        return _postRepository.Create(post);
    }

    // Ostavlja reakciju (like/dislike) na komentar.
    // - Ako serviser već ima istu reakciju - ne radi ništa.
    // - Ako serviser već ima suprotnu reakciju - menja je.
    // - Ako serviser nema reakciju - kreira novu.
    public void React(ForumPost post, ServiceProvider reactor, bool isUpvote)
    {
        var existing = _reactionRepository.GetByPostAndReactor(post.Id, reactor.Id);

        if (existing is null)
        {
            var reaction = new PostReaction(post, reactor, isUpvote);
            _reactionRepository.Create(reaction);
            return;
        }

        // Ista reakcija već postoji — ne radi ništa
        if (existing.IsUpvote == isUpvote)
            return;

        // Suprotna reakcija postoji — promeni je
        _reactionRepository.Update(existing.Id, isUpvote);
    }

    // Uklanja postojeću reakciju servisera sa komentara.
    // Ako reakcija ne postoji, ne vraća grešku.
    public void RemoveReaction(long postId, long reactorId)
    {
        var existing = _reactionRepository.GetByPostAndReactor(postId, reactorId);
        if (existing is null) return;

        _reactionRepository.Delete(existing.Id);
    }
}