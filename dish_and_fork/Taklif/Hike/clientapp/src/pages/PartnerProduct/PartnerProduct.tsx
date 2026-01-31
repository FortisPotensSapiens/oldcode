import { FC } from 'react';

import { useGetGoodById } from '~/api';
import { Error, LoadingSpinner } from '~/components';
import { usePartnerProductParams } from '~/routing';
import { PartnerProductView } from '~/views';

import { useSubmit } from './useSubmit';

const PartnerProduct: FC = () => {
  const { productId = '' } = usePartnerProductParams();
  const { data, isError, isLoading } = useGetGoodById(productId);
  const [submitHandler, { isLoading: isPatching }] = useSubmit(productId);

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (isError || !data) {
    return <Error />;
  }

  return <PartnerProductView disabled={isPatching} merchandise={data} onSubmit={submitHandler} />;
};

export { PartnerProduct };
