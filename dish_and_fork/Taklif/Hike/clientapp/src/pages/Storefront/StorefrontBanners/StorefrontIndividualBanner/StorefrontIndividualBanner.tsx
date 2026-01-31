import { useNavigate } from 'react-router-dom';

import { getIndividualOrdersPath } from '~/routing';

import { BannerContainer, BannerTitle } from '../styled';
import { StyledButton } from './styled';

const StorefrontIndividualBanner = () => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(getIndividualOrdersPath());
  };

  return (
    <BannerContainer>
      <BannerTitle color="#7e004a">Хочется чего-то особенного?</BannerTitle>
      <StyledButton onClick={handleClick}>Нажимай!</StyledButton>
    </BannerContainer>
  );
};

export { StorefrontIndividualBanner };
