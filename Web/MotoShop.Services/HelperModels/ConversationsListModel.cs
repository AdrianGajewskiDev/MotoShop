using System.Collections.Generic;

namespace MotoShop.Services.HelperModels
{
    public record ConversationsListModel
    {
        public IEnumerable<ConversationListItemModel> Conversations { get; set; }
    }
}
