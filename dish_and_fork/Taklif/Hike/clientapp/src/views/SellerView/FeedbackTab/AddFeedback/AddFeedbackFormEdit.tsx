import { Spin } from 'antd';
import { useEffect } from 'react';

import { usePutUpgradeRating } from '~/api/v1/usePutUpgradeRating';
import { MerchRatingCreateModel, MerchRatingReadModel } from '~/types';

import AddFeedbackForm from './AddFeedbackForm';

const AddFeedbackFormEdit = ({
  productId,
  onSuccess,
  initValues,
}: {
  productId: string;
  onSuccess: () => void;
  initValues: MerchRatingReadModel;
}) => {
  const mutation = usePutUpgradeRating();

  const send = (data: MerchRatingCreateModel) => {
    mutation.mutate({
      rating: data.rating,
      comment: data.comment,
      id: initValues.id,
    });
  };

  useEffect(() => {
    if (mutation.isSuccess) {
      onSuccess();
    }
  }, [mutation.isSuccess, onSuccess]);

  if (mutation.isLoading) {
    return <Spin />;
  }

  return (
    <AddFeedbackForm
      submitButtonText="Редактировать отзыв"
      productId={productId}
      onSubmit={send}
      loading={mutation.isLoading}
      defaultValues={initValues}
    />
  );
};

export default AddFeedbackFormEdit;
