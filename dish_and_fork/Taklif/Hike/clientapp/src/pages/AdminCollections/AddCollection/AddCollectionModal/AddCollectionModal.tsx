import styled from '@emotion/styled';
import { yupResolver } from '@hookform/resolvers/yup';
import { Button, Input, Modal } from 'antd';
import { FC, useEffect } from 'react';
import { Controller, useForm } from 'react-hook-form';
import * as yup from 'yup';

import CategoriesSelect from '~/components/CategoriesSelect/CategoriesSelect';
import FloatLabel from '~/components/FloatLabel/FloatLabel';
import { CategoryReadModel, CollectionCreateModel, CollectionUpdateModel } from '~/types';
import { CollectionReadModel } from '~/types/collection';

const ButtonsContainer = styled.div`
  margin-top: 1rem;
  display: flex;
  justify-content: space-between;
`;

const FieldLine = styled.div`
  margin-bottom: 0.5rem;
`;

const validationSchema = yup.object({
  title: yup.string().required(),
});

const AddCollectionModal: FC<{
  open: boolean;
  onClose: () => void;
  onSubmitForm: (data: CollectionCreateModel | CollectionUpdateModel) => void;
  categories: CategoryReadModel[] | null | undefined;
  loading: boolean;
  modalTitle: string;
  okTitle: string;
  defaultValues?: CollectionReadModel;
}> = ({ open, onClose, onSubmitForm, categories, loading, defaultValues, modalTitle, okTitle }) => {
  const { handleSubmit, control, formState, reset } = useForm<CollectionCreateModel>({
    mode: 'all',
    resolver: yupResolver(validationSchema),
  });

  useEffect(() => {
    if (defaultValues) {
      reset({
        title: defaultValues?.title,
        categories: defaultValues?.categories.map((category) => {
          return category.id;
        }),
      });
    }
  }, [defaultValues, reset]);

  const onSubmit = (data: CollectionCreateModel) => {
    let submitData;

    if (defaultValues) {
      submitData = { ...data, id: defaultValues.id };
    } else {
      submitData = { ...data };
    }
    onSubmitForm(submitData);
  };

  const onCloseModal = () => {
    onClose();
    reset();
  };

  return (
    <Modal title={modalTitle} open={open} footer={null} onCancel={onClose}>
      <form>
        <FieldLine>
          <Controller
            control={control}
            name="title"
            render={({ field }) => {
              return (
                <FloatLabel label="Название" required value={field.value}>
                  <Input {...field} />
                </FloatLabel>
              );
            }}
            rules={{ required: true }}
          />
        </FieldLine>

        {categories?.length ? (
          <FieldLine>
            <Controller
              control={control}
              name="categories"
              render={({ field }) => <CategoriesSelect categories={categories ?? []} {...field} />}
            />
          </FieldLine>
        ) : undefined}

        <ButtonsContainer>
          <Button loading={loading} disabled={!formState.isValid} onClick={handleSubmit(onSubmit)} type="primary">
            {okTitle}
          </Button>
          <Button onClick={onCloseModal}>Закрыть</Button>
        </ButtonsContainer>
      </form>
    </Modal>
  );
};

export { AddCollectionModal };
