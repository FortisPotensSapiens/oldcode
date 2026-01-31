import { Spin } from 'antd';
import { useContext } from 'react';

import { useGetCollections } from '~/api/v1/useGetCollections';
import { CollectionReadModel } from '~/types/collection';

import { StorefrontContext } from '../../Storefront.context';
import { StorefrontCollectionsList } from './StorefrontCollectionsList';
import { StyledSpin } from './styled';

const StoreFrontCollectionsListContainer = () => {
  const { isFetching, data } = useGetCollections();
  const storefrontContext = useContext(StorefrontContext);

  const handleCollectionItemClick = (collection: CollectionReadModel) => {
    if (collection.id !== storefrontContext.selectedCollection?.id) {
      storefrontContext.updateSelectedCollection(collection);
    } else {
      storefrontContext.dropCollection();
    }
  };

  if (isFetching) {
    return <Spin indicator={<StyledSpin />} />;
  }

  return (
    <StorefrontCollectionsList
      collections={data ?? []}
      selectedCollection={storefrontContext.selectedCollection}
      onCollectionClick={handleCollectionItemClick}
    />
  );
};

export { StoreFrontCollectionsListContainer };
