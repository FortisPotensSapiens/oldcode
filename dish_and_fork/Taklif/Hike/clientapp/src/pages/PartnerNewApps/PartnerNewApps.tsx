import { Box, Grid } from '@mui/material';
import { FC } from 'react';

import { useGetSellerIndividualApplications } from '~/api';
import { EmptyList, Error, LoadingSpinner, PageLayout, Pagination } from '~/components';
import { useConfig } from '~/contexts';
import { usePageNumber } from '~/hooks';

import { NewAppItem } from './NewAppItem';
import { useRedirect } from './useRedirect';

const PartnerNewApps: FC = () => {
  const [pageNumber] = usePageNumber();
  const { applications } = useConfig();
  const { pageSize } = applications;
  const { data, isError, isLoading } = useGetSellerIndividualApplications(pageNumber, pageSize);
  const redirect = useRedirect();

  if (isError) {
    return <Error />;
  }

  if (isLoading) {
    return <LoadingSpinner />;
  }

  const pagesCount = data && Math.ceil(data.totalCount / pageSize);

  return (
    <PageLayout title="Новые заявки">
      {!data?.totalCount ? (
        <EmptyList>Заявок нет</EmptyList>
      ) : (
        <>
          <Grid container spacing={{ sm: 3, xs: 2 }}>
            {data?.items?.map((item) => (
              <Grid key={item.id} item md={4} sm={6} xs={12}>
                <NewAppItem {...item} onComplete={redirect} />
              </Grid>
            ))}
          </Grid>

          <Box display="flex" justifyContent="center" mb={1} mt={{ md: 3, xs: 2 }}>
            <Pagination pageNumber={pageNumber} pagesCount={pagesCount} />
          </Box>
        </>
      )}
    </PageLayout>
  );
};

export { PartnerNewApps };
