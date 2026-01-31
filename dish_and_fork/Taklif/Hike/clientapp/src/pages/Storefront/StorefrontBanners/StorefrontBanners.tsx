import { StorefrontCollectionsBanner } from './StoreFrontCollectionsBanner/StorefrontCollectionsBanner';
import { StorefrontIndividualBanner } from './StorefrontIndividualBanner/StorefrontIndividualBanner';
import { Banner, StorefrontBannersContainer } from './styled';

const StorefrontBanners = () => {
  return (
    <StorefrontBannersContainer>
      <Banner bgColor="#e1c6d6">
        <StorefrontIndividualBanner />
      </Banner>
      <Banner bgColor="#7e004a">
        <StorefrontCollectionsBanner />
      </Banner>
    </StorefrontBannersContainer>
  );
};

export { StorefrontBanners };
