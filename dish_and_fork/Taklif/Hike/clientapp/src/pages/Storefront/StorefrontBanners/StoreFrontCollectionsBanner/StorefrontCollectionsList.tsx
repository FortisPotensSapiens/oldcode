import { FC } from 'react';

import { CollectionReadModel } from '~/types/collection';

import { StorefrontCollectionsListItem } from './StorefrontCollectionsListItem/StorefrontCollectionsListItem';
import { Container } from './styled';

const StorefrontCollectionsList: FC<{
  collections: CollectionReadModel[];
  selectedCollection: CollectionReadModel | undefined | null;
  onCollectionClick: (collection: CollectionReadModel) => void;
}> = ({ collections, onCollectionClick, selectedCollection }) => {
  return (
    <Container>
      {collections.map((collection) => {
        return (
          <StorefrontCollectionsListItem
            selected={selectedCollection?.id === collection.id}
            onClick={onCollectionClick}
            collection={collection}
            key={collection.id}
          />
        );
      })}
    </Container>
  );
};

export { StorefrontCollectionsList };
