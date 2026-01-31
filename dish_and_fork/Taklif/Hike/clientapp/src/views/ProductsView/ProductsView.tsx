import { Typography } from '@mui/material';
import { FC } from 'react';
import { UseQueryResult } from 'react-query';

import { Error, Pagination, StorefrontLayout } from '~/components';
import { useConfig } from '~/contexts';
import { usePageNumber } from '~/hooks';
import { MerchandiseReadModelPageResultModel } from '~/types';

import { ProductsList, ProductsListProps } from './ProductsList';

type ProductsViewProps = UseQueryResult<MerchandiseReadModelPageResultModel> &
  Pick<ProductsListProps, 'hideSeller'> & { isGlobalLoading?: boolean };

const ProductsView: FC<ProductsViewProps> = ({ children, data, error, hideSeller, isGlobalLoading, isLoading }) => {
  const [pageNumber] = usePageNumber();
  const { storefront } = useConfig();
  const { pageSize } = storefront;

  if (error) {
    return <Error />;
  }

  const pagesCount = data && Math.ceil(data.totalCount / pageSize);

  return (
    <StorefrontLayout
      isLoading={isGlobalLoading || isLoading}
      pagination={<Pagination pageNumber={pageNumber} pagesCount={pagesCount} />}
      title={children}
      height="auto"
    >
      {!isLoading ? (
        <>
          {data?.items && <ProductsList hideSeller={hideSeller} items={data.items} />}
          {!data?.items?.length && <Typography>Нет продуктов</Typography>}
        </>
      ) : undefined}
    </StorefrontLayout>
  );
};

export { ProductsView };
