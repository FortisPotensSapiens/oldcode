import { Box, Typography } from '@mui/material';
import { FC } from 'react';
import { UseQueryResult } from 'react-query';

import { EmptyList, Error, Link, LoadingSpinner, PageLayout } from '~/components';
import { generateSellerPath } from '~/routing';
import { PartnerReadModel } from '~/types';

import ProductRating from '../ProductsView/ProductsList/ProductsListItem/ProductRating';
import { StyledRatingContainer } from './styled';
import StyledCardLink from './StyledCardLink';
import StyledGrid from './StyledGrid';

const variantMapping = { h5: 'div' };

type SellersViewProps = UseQueryResult<PartnerReadModel[]>;

const SellersView: FC<SellersViewProps> = ({ data, error, isLoading }) => {
  if (error) {
    return <Error />;
  }

  if (isLoading) {
    return <LoadingSpinner />;
  }

  const noData = !data || data.length < 1;

  return (
    <PageLayout title="Список продавцов">
      {noData ? (
        <EmptyList>Список продавцов пуст</EmptyList>
      ) : (
        <StyledGrid>
          {data.map(({ id, image, title, rating }) => (
            <Box
              key={id}
              component={Link}
              height={1}
              sx={{ textDecoration: 'none' }}
              to={generateSellerPath({ sellerId: id })}
            >
              <StyledCardLink>
                <Box
                  bgcolor="grey.300"
                  borderRadius="50%"
                  height={120}
                  width={120}
                  overflow="hidden"
                  style={image?.path ? { backgroundImage: `url(${image?.path})` } : undefined}
                  sx={{
                    backgroundPosition: 'center center',
                    backgroundSize: 'contain',
                  }}
                />

                <Typography
                  color="text.primary"
                  mt={1.5}
                  overflow="hidden"
                  textAlign="center"
                  variant="h5"
                  fontSize="130%"
                  variantMapping={variantMapping}
                >
                  <Box textOverflow="ellipsis" width={1}>
                    {title}
                    <StyledRatingContainer>
                      <ProductRating rating={rating} />
                    </StyledRatingContainer>
                  </Box>
                </Typography>
              </StyledCardLink>
            </Box>
          ))}
        </StyledGrid>
      )}
    </PageLayout>
  );
};

export { SellersView };
export type { SellersViewProps };
