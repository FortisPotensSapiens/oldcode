import { Alert, notification, Spin } from 'antd';
import { FC, useEffect, useState } from 'react';

import { useDeleteAdminCollection } from '~/api/v1/useDeleteAdminCollection';
import { useGetAdminCollections } from '~/api/v1/useGetAdminCollections';
import { usePostCategoriesFilter } from '~/api/v1/usePostCategoriesByFilter';
import { CollectionReadModel } from '~/types/collection';
import { useCategories } from '~/utils/categories';

import { AddCollectionContainer } from '../AddCollection/AddCollectionContainer';
import { UpdateCollectionContainer } from '../UpdateCollection/UpdateCollectionContainer';
import { CollectionsList } from './CollectionsList';

const CollectionsListContainer: FC = () => {
  const { data: collections, isLoading, refetch } = useGetAdminCollections();
  const deleteMutation = useDeleteAdminCollection();
  const [api, contextHolder] = notification.useNotification();
  const [editCollection, setEditCollection] = useState<CollectionReadModel | null>(null);
  const { data: allCategories, isFetching } = usePostCategoriesFilter({
    pageNumber: 1,
    pageSize: 500,
    hideEmpty: false,
  });

  const [categories] = useCategories(allCategories?.items);

  const onDeleteCollection = (id: string) => {
    deleteMutation.mutate({
      id,
    });
  };

  const onEditCollection = (collection: CollectionReadModel) => {
    setEditCollection(collection);
  };

  const onEditClose = () => {
    setEditCollection(null);
  };

  const onCollectionAdded = () => {
    refetch();
  };

  const onEdited = () => {
    refetch();
  };

  useEffect(() => {
    if (deleteMutation.isSuccess) {
      api.success({
        message: 'Коллекция удалена',
      });
      refetch();
    }
  }, [api, deleteMutation.isSuccess, refetch]);

  if (isLoading || isFetching) {
    return <Spin />;
  }

  return (
    <>
      {contextHolder}
      <AddCollectionContainer onAdded={onCollectionAdded} categories={categories} />

      <CollectionsList
        onEditCollection={onEditCollection}
        onDeleteCollection={onDeleteCollection}
        collections={collections ?? []}
      />

      {!collections?.length ? <Alert type="info" message="Нет доступный коллекций." /> : undefined}

      <UpdateCollectionContainer
        onEdited={onEdited}
        onEditClose={onEditClose}
        categories={categories}
        collection={editCollection}
      />
    </>
  );
};

export { CollectionsListContainer };
