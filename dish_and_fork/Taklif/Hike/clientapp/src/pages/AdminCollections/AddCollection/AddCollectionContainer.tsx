import styled from '@emotion/styled';
import { Button, notification } from 'antd';
import { FC, useEffect, useState } from 'react';
import { v4 as uuidv4 } from 'uuid';

import { usePostAdminCollections } from '~/api/v1/usePostAdminCollections';
import { CategoryReadModel, CollectionCreateModel, CollectionUpdateModel } from '~/types';

import { AddCollectionModal } from './AddCollectionModal/AddCollectionModal';

const Container = styled.div`
  margin-bottom: 1rem;
`;

const AddCollectionContainer: FC<{
  categories: CategoryReadModel[] | null | undefined;
  onAdded?: () => void;
}> = ({ categories, onAdded }) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const mutation = usePostAdminCollections();
  const [api, contextHolder] = notification.useNotification();
  const [formId, setFormId] = useState(uuidv4());

  const onCloseModal = () => {
    setIsModalOpen(false);
  };

  const onShowModal = () => {
    setIsModalOpen(true);
  };

  const onAddCollection = (data: CollectionCreateModel | CollectionUpdateModel) => {
    mutation.mutate(data);
  };

  useEffect(() => {
    if (mutation.isSuccess) {
      onCloseModal();
      setFormId(uuidv4());

      api.success({
        message: `Коллекция создана успешно`,
      });

      onAdded?.();
    }
  }, [api, mutation.isSuccess]);

  useEffect(() => {
    if (mutation.isError) {
      api.error({
        message: `Ошибка создания коллекции. ${(mutation.error as any)?.response?.data?.Message ?? ''}`,
      });
    }
  }, [api, mutation.error, mutation.isError]);

  return (
    <Container>
      {contextHolder}
      <Button type="primary" onClick={onShowModal}>
        Создать коллекцию
      </Button>
      <AddCollectionModal
        key={formId}
        loading={mutation.isLoading}
        open={isModalOpen}
        onClose={onCloseModal}
        onSubmitForm={onAddCollection}
        categories={categories}
        modalTitle="Создать коллекцию"
        okTitle="Добавить"
      />
    </Container>
  );
};

export { AddCollectionContainer };
