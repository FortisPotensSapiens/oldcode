import { Spin } from 'antd';
import { useEffect } from 'react';

import { usePostAddProductFeedback } from '~/api/v1/usePostAddProductFeedback';
import { MerchRatingCreateModel } from '~/types';

import AddFeedbackForm from './AddFeedbackForm';

const AddFeedbackFormAdd = ({ productId, onSuccess }: { productId: string; onSuccess: () => void }) => {
  const mutation = usePostAddProductFeedback();

  const send = (data: MerchRatingCreateModel) => {
    mutation.mutate(data);
  };

  useEffect(() => {
    if (mutation.isSuccess) {
      onSuccess();
    }
  }, [mutation.isSuccess, onSuccess]);

  if (mutation.isLoading) {
    return <Spin />;
  }

  return <AddFeedbackForm productId={productId} onSubmit={send} loading={mutation.isLoading} />;
};

export default AddFeedbackFormAdd;
