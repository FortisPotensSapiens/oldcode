import { StarOutlined } from '@ant-design/icons';
import styled from '@emotion/styled';

const Container = styled.div`
  padding: 3px 10px;
  border-radius: 10px;
  background-color: rgba(255, 255, 255, 0.8);
  z-index: 99;
  font-size: 70%;
`;

const StarContainer = styled.span`
  display: inline-block;
  padding-right: 0.2rem;
`;

const ProductRating = ({ rating, nullText = '' }: { nullText?: string; rating: number | null | undefined }) => {
  if (!rating && !nullText) {
    return <></>;
  }

  return (
    <Container>
      <StarContainer>
        <StarOutlined />
      </StarContainer>
      {rating ? rating.toPrecision(2) : nullText}
    </Container>
  );
};

export default ProductRating;
