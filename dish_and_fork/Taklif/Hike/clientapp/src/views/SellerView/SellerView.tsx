import { FC } from 'react';
import { UseQueryResult } from 'react-query';

import { Error, LoadingSpinner } from '~/components';
import { PartnerReadModel } from '~/types';

import { SellerHeader } from './SellerHeader';

type SellerViewProps = UseQueryResult<PartnerReadModel> & { showSpinner?: boolean };

const SellerView: FC<SellerViewProps> = ({ data, error, isLoading, showSpinner }) => {
  if (error) {
    return <Error />;
  }

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (data) {
    return <SellerHeader {...data} />;
  }

  return <></>;
};

export { SellerView };
