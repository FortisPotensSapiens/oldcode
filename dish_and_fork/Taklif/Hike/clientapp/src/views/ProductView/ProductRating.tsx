import { StarOutlined } from '@ant-design/icons';
import styled from '@emotion/styled';

const Container = styled.div<{ fontSize: string }>`
  padding: 3px 10px;
  border-radius: 10px;
  background-color: rgba(255, 255, 255, 0.8);
  font-size: ${(props) => props.fontSize};
  white-space: nowrap;
  color: #000;
`;

const StarContainer = styled.span`
  display: inline-block;
  padding-right: 0.2rem;
`;

const ProductRating = ({
  rating,
  fontSize = '1.2rem',
  nullText = '',
}: {
  rating: number | null | undefined;
  fontSize?: string;
  nullText?: string;
}) => {
  if (!rating && !nullText) {
    return <></>;
  }

  return (
    <Container fontSize={fontSize}>
      <StarContainer>
        <StarOutlined />
      </StarContainer>
      {rating ? rating.toPrecision(2) : nullText}
    </Container>
  );
};

export default ProductRating;
