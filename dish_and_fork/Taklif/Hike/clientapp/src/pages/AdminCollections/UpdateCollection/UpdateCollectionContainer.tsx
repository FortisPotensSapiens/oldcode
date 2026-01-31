import styled from '@emotion/styled';
import { notification } from 'antd';
import { FC, useEffect, useState } from 'react';
import { v4 as uuidv4 } from 'uuid';

import { usePutAdminCollection } from '~/api/v1/usePutAdminCollection';
import { CategoryReadModel, CollectionCreateModel, CollectionUpdateModel } from '~/types';
import { CollectionReadModel } from '~/types/collection';

import { AddCollectionModal } from '../AddCollection/AddCollectionModal/AddCollectionModal';

const Container = styled.div`
  margin-bottom: 1rem;
`;

const UpdateCollectionContainer: FC<{
  categories: CategoryReadModel[] | null | undefined;
  collection: CollectionReadModel | null;
  onEditClose: () => void;
  onEdited?: () => void;
}> = ({ categories, collection, onEditClose, onEdited }) => {
  const mutation = usePutAdminCollection();
  const [api, contextHolder] = notification.useNotification();
  const [formId, setFormId] = useState(uuidv4());

  const onUpdateCollection = (data: CollectionCreateModel | CollectionUpdateModel) => {
    if ('id' in data) {
      mutation.mutate(data);
    }
  };

  useEffect(() => {
    if (mutation.isSuccess) {
      onEditClose();
      setFormId(uuidv4());

      onEdited?.();

      api.success({
        message: `Коллекция обновлена успешно`,
      });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [mutation.isSuccess]);

  useEffect(() => {
    if (mutation.isError) {
      api.error({
        message: `Ошибка обновления коллекции. ${(mutation.error as any)?.response?.data?.Message ?? ''}`,
      });
    }
  }, [api, mutation.error, mutation.isError]);

  return (
    <Container>
      {contextHolder}
      <AddCollectionModal
        key={formId}
        loading={mutation.isLoading}
        open={!!collection}
        onClose={onEditClose}
        onSubmitForm={onUpdateCollection}
        categories={categories}
        modalTitle="Изменить коллекцию"
        defaultValues={collection ?? undefined}
        okTitle="Изменить"
      />
    </Container>
  );
};

export { UpdateCollectionContainer };
