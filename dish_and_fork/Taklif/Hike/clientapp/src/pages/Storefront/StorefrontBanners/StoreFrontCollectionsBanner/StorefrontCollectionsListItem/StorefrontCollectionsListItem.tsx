import { FC } from 'react';

import { CollectionReadModel } from '~/types/collection';

import { StyledButton } from './styled';

const StorefrontCollectionsListItem: FC<{
  collection: CollectionReadModel;
  onClick: (collection: CollectionReadModel) => void;
  selected: boolean;
}> = ({ collection, onClick, selected }) => {
  const handleClick = () => {
    onClick(collection);
  };

  return (
    <StyledButton selected={selected} onClick={handleClick}>
      {collection.title}
    </StyledButton>
  );
};

export { StorefrontCollectionsListItem };
