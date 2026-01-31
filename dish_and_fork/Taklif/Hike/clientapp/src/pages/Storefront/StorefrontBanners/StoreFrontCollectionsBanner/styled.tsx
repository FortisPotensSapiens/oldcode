import LoadingOutlined from '@ant-design/icons/lib/icons/LoadingOutlined';
import styled from '@emotion/styled';

import { BannerContainer } from '../styled';

const Container = styled.div`
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-column-gap: 10px;
  grid-row-gap: 10px;

  @media (max-width: 1200px) {
    margin-top: 1rem;
  }
`;

const StyledSpin = styled(LoadingOutlined)`
  font-size: 140%;
  color: #fff;
`;

const StyledBannerContainer = styled(BannerContainer)`
  @media (max-width: 1200px) {
    display: block;
  }
`;

export { Container, StyledBannerContainer, StyledSpin };
