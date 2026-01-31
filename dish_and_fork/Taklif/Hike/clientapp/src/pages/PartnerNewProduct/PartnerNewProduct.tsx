import { FC } from 'react';

import { PartnerProductView } from '~/views';

import { useSubmit } from './useSubmit';

const PartnerNewProduct: FC = () => {
  const [submitHandler, { isLoading }] = useSubmit();

  return <PartnerProductView disabled={isLoading} onSubmit={submitHandler} />;
};

export { PartnerNewProduct };
