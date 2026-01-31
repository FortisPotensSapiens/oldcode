import { BannerTitle } from '../styled';
import { StoreFrontCollectionsListContainer } from './StorefrontCollectionsListContainer';
import { StyledBannerContainer } from './styled';

const StorefrontCollectionsBanner = () => {
  return (
    <StyledBannerContainer>
      <BannerTitle color="#fff">Используй готовые подборки!</BannerTitle>
      <StoreFrontCollectionsListContainer />
    </StyledBannerContainer>
  );
};

export { StorefrontCollectionsBanner };
