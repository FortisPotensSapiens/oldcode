import { yupResolver } from '@hookform/resolvers/yup';
import { Modal, Typography } from 'antd';
import TextArea from 'antd/es/input/TextArea';
import { FC, useEffect } from 'react';
import { Controller, useForm } from 'react-hook-form';
import * as yup from 'yup';

import { usePutAdminGoods } from '~/api/v1/usePutAdminGoods';
import { MerchandiseReadModel } from '~/types';

import { AdminCommentForm } from './types';

const validationSheme = yup.object({
  reasonForBlocking: yup.string().required(),
});

const AdminProductsCommentModal: FC<{
  product: MerchandiseReadModel | undefined;
  onClose: () => void;
  onSuccess: () => void;
}> = ({ product, onClose, onSuccess }) => {
  const form = useForm<AdminCommentForm>({
    mode: 'all',
    resolver: yupResolver(validationSheme),
  });
  const mutation = usePutAdminGoods();

  const handleOk = () => {
    const values = form.getValues();

    if (product) {
      mutation.mutateAsync({
        ...product,
        ...values,
        categories:
          product.categories?.map((category) => {
            return category.id;
          }) ?? [],
      });
    }
  };

  const handleCancel = () => {
    form.reset();

    onClose();
  };

  useEffect(() => {
    if (mutation.isSuccess) {
      onSuccess();
    }
  }, [mutation.isSuccess]);

  return (
    <Modal
      onCancel={handleCancel}
      onOk={handleOk}
      okButtonProps={{ loading: mutation.isLoading, disabled: !form.formState.isValid }}
      open={!!product}
      okText="Оставить комментарий"
      cancelText="Закрыть"
    >
      <Typography.Title level={4}>Сообщить о нарушении</Typography.Title>

      <Controller
        name="reasonForBlocking"
        control={form.control}
        render={({ field }) => {
          return <TextArea autoFocus {...field} />;
        }}
      />
    </Modal>
  );
};

export { AdminProductsCommentModal };
