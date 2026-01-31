import { LeftOutlined, RightOutlined } from '@ant-design/icons';
import { Box, Chip, Typography } from '@mui/material';
import { styled } from '@mui/system';
import { Alert } from 'antd';
import getUserLocale from 'get-user-locale';
import Carousel from 'nuka-carousel';
import { FC } from 'react';
import { useNavigate } from 'react-router-dom';

import { CardLink, Link, ToCartBlock } from '~/components';
import { useDownSm } from '~/hooks';
import { generateProductPath, generateSellerPath, generateSellerProductPath } from '~/routing';
import { MerchandiseReadModel, MerchandisesState } from '~/types';
import { getNormalizedServingSize } from '~/utils';

import ImageBox from './ImageBox';
import InfoBox from './InfoBox';
import PriceBox from './PriceBox';
import ProductRating from './ProductRating';

type ProductsListItemProps = {
  merchandise: MerchandiseReadModel;
  hideSeller?: boolean;
  addToCard?: boolean;
  customActions?: (merchandise: MerchandiseReadModel) => JSX.Element;
  showTags?: boolean;
};

const StyledTitle = styled(Box)(({ theme }) => ({
  lineHeight: '140%',
  paddingBottom: '3px',
  flex: 1,
}));

const StyledPartner = styled(Box)(({ theme }) => ({
  fontSize: '80%',
  paddingBottom: '5px',
}));

const IconBox = styled(Box)`
  padding: 10px;
  background: rgba(255, 255, 255, 0.5);
`;

const ProductRatingContainer = styled('div')`
  position: absolute;
  top: 3px;
  right: 3px;
  z-index: 99;
`;

const ProductsListItem: FC<ProductsListItemProps> = ({
  hideSeller,
  merchandise,
  addToCard = true,
  customActions,
  showTags = false,
}) => {
  const { id, images, price, seller, servingSize, title, unitType, categories, rating, isTagsAppovedByAdmin, state } =
    merchandise;
  const navigate = useNavigate();
  const isSmall = useDownSm();

  const to = hideSeller
    ? generateSellerProductPath({ productId: id, sellerId: seller.id })
    : generateProductPath({ productId: id });

  return (
    <CardLink>
      <ProductRatingContainer>
        <ProductRating rating={rating} />
      </ProductRatingContainer>

      {images?.length ? (
        <Carousel
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
              <img
                onClick={() => navigate(to)}
                alt=""
                key={image.id}
                src={String(image.path)}
                height={isSmall ? '150px' : '210px'}
              />
            );
          })}
        </Carousel>
      ) : (
        <ImageBox style={{ height: isSmall ? '150px' : '210px' }} />
      )}

      {state === MerchandisesState.Blocked ? (
        <Alert message="Необходимо изменить" type="error" />
      ) : (
        <>
          {!isTagsAppovedByAdmin ? <Alert message="На модерации" /> : undefined}
          {state !== MerchandisesState.Published ? <Alert message="Не опубликован" /> : undefined}
        </>
      )}

      <PriceBox onClick={() => navigate(to)}>
        <Typography component="div">
          <strong>{price.toLocaleString(getUserLocale(), { style: 'currency', currency: 'RUB' })}</strong>
        </Typography>

        <Typography
          color="grey.400"
          component="div"
          style={{
            fontSize: '80%',
          }}
        >
          за {getNormalizedServingSize(servingSize, unitType)}
        </Typography>
      </PriceBox>

      <InfoBox onClick={() => navigate(to)}>
        <StyledTitle>
          <Link color="text.primary" sx={{ textDecoration: 'none' }} to={to}>
            {title}
          </Link>
        </StyledTitle>

        {categories && showTags ? (
          <Box
            style={{
              overflow: 'scroll',
              flexWrap: 'nowrap',
              width: '100%',
              whiteSpace: 'nowrap',
              display: 'flex',
              paddingBottom: '15px',
            }}
            onClick={(e) => {
              e.preventDefault();
              e.stopPropagation();
            }}
          >
            {categories.map((category) => {
              return (
                <Chip
                  key={category.id}
                  label={category.title}
                  style={{
                    marginRight: '10px',
                  }}
                />
              );
            })}
          </Box>
        ) : undefined}

        {!hideSeller && (
          <StyledPartner>
            <Link
              color="text.secondary"
              onClick={(event) => event.stopPropagation()}
              to={generateSellerPath({ sellerId: seller.id })}
            >
              {seller.title}
            </Link>
          </StyledPartner>
        )}

        {addToCard ? <ToCartBlock merchandise={merchandise} /> : undefined}
        {customActions ? customActions(merchandise) : undefined}
      </InfoBox>
    </CardLink>
  );
};

export type { ProductsListItemProps };
export { ProductsListItem };
