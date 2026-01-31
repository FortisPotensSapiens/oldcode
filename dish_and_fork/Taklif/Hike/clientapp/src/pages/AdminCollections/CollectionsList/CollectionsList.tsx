import { FC } from 'react';

import { CollectionReadModel } from '~/types/collection';

import { CollectionsListItem } from './CollectionsListItem/CollectionsListItem';
import { CellItem, Container } from './types';

const CollectionsList: FC<{
  collections: CollectionReadModel[];
  onDeleteCollection: (id: string) => void;
  onEditCollection: (collection: CollectionReadModel) => void;
}> = ({ collections, onDeleteCollection, onEditCollection }) => {
  return (
    <Container>
      {collections.map((collection) => {
        return (
          <CellItem key={collection.id}>
            <CollectionsListItem
              onEditCollection={onEditCollection}
              onDeleteCollection={onDeleteCollection}
              collection={collection}
            />
          </CellItem>
        );
      })}
    </Container>
  );
};

export { CollectionsList };
