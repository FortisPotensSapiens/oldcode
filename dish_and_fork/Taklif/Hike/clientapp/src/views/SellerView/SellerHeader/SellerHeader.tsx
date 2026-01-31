import { FC } from 'react';

import { ShowMoreTextTextField, SmBackButton } from '~/components';
import { getSellersPath } from '~/routing';
import { PartnerReadModel } from '~/types';
import ProductRating from '~/views/ProductView/ProductRating';

import StyledFone from './StyledFone';
import StyledFrame from './StyledFrame';
import StyledImage from './StyledImage';
import StyledInfo from './StyledInfo';
import StyledTypography from './StyledTypography';

type SellerHeaderProps = PartnerReadModel;

const variantMapping = { h5: 'h1' };

const SellerHeader: FC<SellerHeaderProps> = ({ children, description, image, title, rating }) => (
  <>
    <StyledFone />
    <SmBackButton href={getSellersPath()} />

    <StyledFrame>
      <StyledImage style={image?.path ? { backgroundImage: `url(${image.path})` } : undefined} />

      <StyledInfo>
        <StyledTypography color="text.secondary" gridArea="type" lineHeight={{ sm: 2 }}>
          <div>Кондитер</div>
          <ProductRating rating={rating} />
        </StyledTypography>

        <StyledTypography flexGrow="1" gridArea="title" variant="h5" variantMapping={variantMapping}>
          {title}
        </StyledTypography>

        {description && (
          <StyledTypography gridArea="description" mt={{ sm: 0, xs: 1.25 }}>
            <ShowMoreTextTextField>{description}</ShowMoreTextTextField>
          </StyledTypography>
        )}
      </StyledInfo>
    </StyledFrame>

    {children}
  </>
);

export { SellerHeader };
export type { SellerHeaderProps };
