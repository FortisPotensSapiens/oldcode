import { Modal } from 'antd';
import { useEffect } from 'react';
import { Controller, FormProvider, useForm } from 'react-hook-form';

import { useGetGoodById } from '~/api';
import { usePostCategoriesFilter } from '~/api/v1/usePostCategoriesByFilter';
import { usePutAdminGoods } from '~/api/v1/usePutAdminGoods';
import { LoadingSpinner } from '~/components';
import CategoriesSelect from '~/components/CategoriesSelect/CategoriesSelect';

interface FormValues {
  categories: string[] | undefined;
}

const AdminProductsEditModal = ({
  editId = false,
  onClose,
  onSuccess,
}: {
  editId: boolean | string;
  onClose: () => void;
  onSuccess: () => void;
}) => {
  const { data, isFetched } = useGetGoodById(String(editId));
  const methods = useForm<FormValues>();
  const categories = usePostCategoriesFilter({ pageNumber: 1, pageSize: 500, hideEmpty: false });
  const mutation = usePutAdminGoods();

  useEffect(() => {
    if (data) {
      methods.reset({
        categories: data?.categories?.map((item) => {
          return item.id;
        }),
      });
    }
  }, [data, methods]);

  useEffect(() => {
    if (mutation.isSuccess) {
      mutation.reset();
      onSuccess();
    }
  }, [mutation, mutation.isSuccess, onSuccess]);

  const onOk = () => {
    const values = methods.getValues();
    mutation.mutate({
      id: String(editId),
      categories: values.categories ?? [],
      isTagsAppovedByAdmin: data?.isTagsAppovedByAdmin ?? false,
    });
  };

  return isFetched ? (
    <Modal title={`Редактирование товара: ${data?.title}`} open={!!editId} onCancel={onClose} onOk={onOk}>
      <FormProvider {...methods}>
        {categories.data?.items ? (
          <Controller
            control={methods.control}
            name="categories"
            render={({ field }) => {
              return <CategoriesSelect categories={categories.data?.items ?? []} {...field} />;
            }}
          />
        ) : undefined}
        {mutation.isLoading ? <LoadingSpinner /> : null}
      </FormProvider>
    </Modal>
  ) : (
    <></>
  );
};

export default AdminProductsEditModal;
