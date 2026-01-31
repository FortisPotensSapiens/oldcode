/* eslint-disable react/jsx-no-undef */
import { LeftOutlined, RightOutlined } from '@ant-design/icons';
import styled from '@emotion/styled';
import { Avatar, Box, Chip, Grid, Typography, useTheme } from '@mui/material';
import { Alert, Button } from 'antd';
import Carousel from 'nuka-carousel';
import { FC, useCallback, useMemo, useState } from 'react';
import { UseQueryResult } from 'react-query';

import { useGetPartnerMy } from '~/api';
import {
  Breadcrumbs,
  Error,
  ImagesThumbs,
  Link,
  LoadingSpinner,
  ShowMoreTextTextField,
  SmBackButton,
  ToCartBlock,
} from '~/components';
import { useCurrencySymbol, useDownSm } from '~/hooks';
import { generateSellerPath, getStorefrontPath, useSellerProductParams } from '~/routing';
import { FileReadModel, MerchandiseReadModel, MerchandiseUnitType } from '~/types';
import { getNormalizedServingSize } from '~/utils';
import { useCategories } from '~/utils/categories';

import { ImagesModal } from './ImagesModal';
import ProductRating from './ProductRating';
import ProductViewTabs from './ProductViewTabs';
import useThumb from './useThumb';

const StyledTitleContainer = styled.div`
  display: flex;
`;

const CompositionTitle = styled(Typography)`
  padding-right: 2px;
  font-size: 90%;
  font-style: italic;
`;

const CompositionChip = styled(Chip)`
  border: 0;
  padding: 0;
  font-style: italic;
  display: inline-block;
  height: auto;

  span {
    padding: 0;
  }
`;

const IconBox = styled(Box)`
  padding: 10px;
  background: rgba(255, 255, 255, 0.5);
`;

const StyledCarousel = styled(Carousel)`
  .slide {
    text-align: center;
  }
`;

const StyledSeparator = styled('span')`
  padding-right: 3px;
`;

type ProductViewProps = UseQueryResult<MerchandiseReadModel> & {
  hideSeller?: boolean;
  onRequestComposition?: (goodId: string) => void;
  userId?: string | null | undefined;
  isCompositionRequestLoading?: boolean;
};

const ProductView: FC<ProductViewProps> = ({
  data,
  error,
  hideSeller,
  isLoading,
  onRequestComposition,
  userId,
  isCompositionRequestLoading,
}) => {
  const currency = useCurrencySymbol();
  const { sellerId } = useSellerProductParams();
  const [image, change] = useThumb(0);
  const isXs = useDownSm();
  const { shadows, spacing } = useTheme();
  const images = useMemo<FileReadModel[]>(() => data?.images || [], [data?.images]);
  const sm = spacing(1.25); // border radius
  const { categories } = data ?? {};
  const { data: partner } = useGetPartnerMy();
  const [openImagesModal, updateOpenImagesModal] = useState(false);

  const [categoriesWithoutComposition, compositions] = useCategories(categories);

  const handleRequestComposition = useCallback(() => {
    if (data?.id) {
      onRequestComposition?.(data.id);
    }
  }, [data?.id, onRequestComposition]);

  const isRequested = useMemo(() => {
    return userId ? data?.compositionRequesters?.includes(userId) : false;
  }, [data?.compositionRequesters, userId]);

  if (error) {
    return <Error />;
  }

  if (isLoading || !data) {
    return <LoadingSpinner />;
  }

  const {
    description,
    price,
    seller,
    servingSize,
    title,
    unitType,
    rating,
    id,
    compositionRequesters,
    servingGrossWeightInKilograms,
  } = data;
  const normalizedServingSize = getNormalizedServingSize(servingSize, unitType);
  const link = sellerId ? generateSellerPath({ sellerId }) : getStorefrontPath();

  const handleImageClick = () => {
    updateOpenImagesModal(true);
  };

  const handleCloseModal = () => {
    updateOpenImagesModal(false);
  };

  return (
    <>
      <ImagesModal images={images} open={openImagesModal} current={image} onCancel={handleCloseModal} />

      <Breadcrumbs>
        <Link key="root" color="text.secondary" sx={{ textDecoration: 'none' }} to={link}>
          {sellerId ? seller.title : 'Все изделия'}
        </Link>

        <Box key="title" color="text.primary">
          {unitType === 'Pieces' ? title : `${title} ${normalizedServingSize}`}
        </Box>
      </Breadcrumbs>

      <Box display={{ sm: 'flex' }} pb={{ sm: 3 }} position="relative" width={1}>
        {images?.length > 0 && (
          <Box display={{ sm: 'block', xs: 'none' }} mr={3}>
            <ImagesThumbs items={images} onClick={change} selected={image} vertical />
          </Box>
        )}

        <Box
          bgcolor="common.white"
          borderRadius={{ sm }}
          boxShadow={shadows[1]}
          display={{ md: 'flex', xs: 'block' }}
          flexWrap={{ md: 'nowrap', sm: 'wrap' }}
          overflow="hidden"
          position="relative"
          width={1}
        >
          <SmBackButton href={link} />

          {!isXs ? (
            <Box flexBasis={{ lg: 544, md: 408 }} flexShrink={0} height={{ sm: 'auto', xs: '234px' }} width={{ xs: 1 }}>
              <Box
                onClick={handleImageClick}
                pb={{ lg: '75%', md: '100%', sm: '75%' }}
                style={{
                  backgroundImage: `url(${images[image]?.path})`,
                  backgroundRepeat: 'no-repeat',
                  height: isXs ? '234px' : undefined,
                  cursor: 'pointer',
                }}
                sx={{
                  backgroundPosition: 'center center',
                  backgroundSize: 'contain',
                  borderBottomLeftRadius: { md: sm },
                  borderTopLeftRadius: { sm },
                }}
                width="100%"
              />
            </Box>
          ) : (
            <StyledCarousel
              cellAlign="center"
              renderCenterLeftControls={(props) => {
                return !props.previousDisabled ? (
                  <IconBox onClick={() => props.previousSlide()}>
                    <LeftOutlined />
                  </IconBox>
                ) : (
                  <></>
                );
              }}
              renderCenterRightControls={(props) => {
                return !props.nextDisabled ? (
                  <IconBox onClick={() => props.nextSlide()}>
                    <RightOutlined />
                  </IconBox>
                ) : (
                  <></>
                );
              }}
            >
              {images?.map((image) => {
                return (
                  // eslint-disable-next-line jsx-a11y/click-events-have-key-events, jsx-a11y/no-noninteractive-element-interactions
                  <img alt="" onClick={handleImageClick} key={image.id} src={String(image.path)} height="210px" />
                );
              })}
            </StyledCarousel>
          )}

          <Box
            display="flex"
            flexDirection="column"
            flexGrow={1}
            ml={{ sm: 3 }}
            pb={{ sm: 4, xs: 3 }}
            pt={{ sm: 4, xs: 2 }}
            px={{ sm: 0, xs: 2 }}
          >
            <Box flexGrow={1} paddingRight={2}>
              <StyledTitleContainer>
                <Typography color="common.black" variant={isXs ? 'h5' : 'h3'} variantMapping={{ h3: 'h1', h5: 'h1' }}>
                  {title}
                </Typography>
                <ProductRating rating={rating} nullText="Новинка" />
              </StyledTitleContainer>

              {categoriesWithoutComposition.length ? (
                <Grid paddingBottom={1} container paddingTop={1}>
                  {categoriesWithoutComposition.map((category) => {
                    return (
                      <Grid item key={category.id} paddingRight={1} paddingBottom={1}>
                        <Chip label={category.title} />
                      </Grid>
                    );
                  })}
                </Grid>
              ) : undefined}

              {compositions.length ? (
                <Grid paddingBottom={1} container alignItems="baseline">
                  <CompositionTitle>Состав:</CompositionTitle>
                  {compositions.map((category, index) => {
                    return (
                      <>
                        {index !== 0 ? <StyledSeparator>,</StyledSeparator> : undefined}
                        <CompositionChip variant="outlined" label={category.title} />
                      </>
                    );
                  })}
                </Grid>
              ) : userId ? (
                seller.id !== partner?.id ? (
                  <Button
                    disabled={isRequested}
                    loading={isCompositionRequestLoading}
                    onClick={handleRequestComposition}
                  >
                    {isRequested
                      ? 'Вы рекомендовали продавцу указать состав'
                      : 'Рекомендовать продавцу указывать состав'}
                  </Button>
                ) : compositionRequesters?.length ? (
                  <Alert
                    type="info"
                    message={
                      <>
                        Количество пользователей запросивших детальный состав:{' '}
                        <strong>{compositionRequesters.length}</strong>
                      </>
                    }
                  />
                ) : (
                  <></>
                )
              ) : undefined}

              <Typography color="common.black" lineHeight={1.4} mt={2}>
                <ShowMoreTextTextField>{description}</ShowMoreTextTextField>
              </Typography>

              {unitType !== 'Pieces' && (
                <Typography color="grey.400" mt={2}>
                  {normalizedServingSize}
                </Typography>
              )}

              <Typography color="grey.400" style={{ fontSize: '80%' }} mt={2}>
                Вес брутто:{getNormalizedServingSize(servingGrossWeightInKilograms, MerchandiseUnitType.Kilograms)}
              </Typography>
            </Box>

            {!hideSeller && (
              <Box
                component={Link}
                display="flex"
                mt={{ sm: 2.5, xs: 2 }}
                sx={{
                  '&:hover': {
                    textDecoration: 'none',
                  },

                  textDecoration: 'none',
                }}
                to={generateSellerPath({ sellerId: seller.id })}
              >
                <Avatar src={seller.image?.path ?? ''}>{(seller.title ?? '').substring(0, 1)}</Avatar>

                <Box ml={2}>
                  <StyledTitleContainer>
                    <Typography color="text.primary">{seller.title}</Typography>

                    <ProductRating rating={seller.rating} fontSize="0.8rem" />
                  </StyledTitleContainer>

                  <Typography color="text.secondary" variant="body2">
                    Продавец
                  </Typography>
                </Box>
              </Box>
            )}

            <Box alignItems="center" display="flex" mt={{ sm: 4, xs: 3 }}>
              <Box color="text.lightGrey" flexGrow={{ sm: 0, xs: 1 }} mr={3} whiteSpace="nowrap">
                <Typography
                  color="common.black"
                  fontSize="1.125rem"
                  fontWeight={700}
                  lineHeight={1.2222222}
                  variantMapping={{ body1: 'span' }}
                >
                  {price.toLocaleString()} {currency}
                </Typography>
                &nbsp; /{normalizedServingSize}
              </Box>

              <ToCartBlock merchandise={data} />
            </Box>
          </Box>
        </Box>
      </Box>

      {id ? <ProductViewTabs productId={id} /> : undefined}
    </>
  );
};

export { ProductView };
export type { ProductViewProps };
