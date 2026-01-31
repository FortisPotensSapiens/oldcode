import styled from '@emotion/styled';

const StorefrontBannersContainer = styled.div`
  display: flex;
  margin-bottom: 1rem;
  column-gap: 10px;

  @media (max-width: 1000px) {
    flex-wrap: wrap;
    margin-top: 0.3rem;
  }
`;

const Banner = styled.div<{ bgColor: string }>`
  border-radius: 15px;
  background-color: ${(props) => props.bgColor};
  padding: 20px 30px;
  flex: 1;
  display: flex;

  @media (max-width: 1000px) {
    margin-bottom: 1rem;
  }
`;

const BannerContainer = styled.div`
  display: flex;
  align-items: center;
  flex: 1;
`;

const BannerTitle = styled.div<{ color: string }>`
  color: ${(props) => props.color};
  line-height: 120%;
  font-size: 140%;
  flex: 1;
`;

export { Banner, BannerContainer, BannerTitle, StorefrontBannersContainer };
