require('rc-slider/assets/index.css');

import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import Slider from 'rc-slider';
import { connect } from 'react-redux';
import { fetchReferences, deleteAllReference, putReference } from '../../common/api/actions';

@connect(null, { putReference })
export default class ImagePair extends Component {

  static propTypes = {
    cannyTreshodLinking: RPT.number,
    cannyTreshold: RPT.number,
    contourUrl: RPT.string,
    id: RPT.string,
    imageUrl: RPT.string,
    putReference: RPT.func,
    pairType: RPT.oneOf(['reference', 'image'])
  }

  render() {
    const { id, imageUrl, contourUrl, cannyTreshodLinking, cannyTreshold, putReference } = this.props;
    return (
      <div style={styles.imageWrapper}>
        <img src={imageUrl} style={styles.image} />
        <img src={`${contourUrl}?${Math.random()}`} style={styles.image} />
        <p>cannyThreshold</p>
        <Slider
          max={250}
          min={5}
          step={5}
          onAfterChange={(x) => pairType === 'reference'
            ? putReference(id, x, cannyTreshodLinking)
            : putImage(id, x, cannyTreshodLinking)}
          defaultValue={cannyTreshold}
        />
        <p>cannyThresholdLinking</p>
        <Slider
          max={250}
          min={5}
          step={5}
          onAfterChange={(x) => pairType === 'reference'
            ? putReference(id, cannyTreshold, x)
            : putImage(id, cannyTreshold, x)}
          defaultValue={cannyTreshodLinking}
        />
      </div>
    );
  }
}

const styles = {
  imageWrapper: {
    clear: 'both'
  },
  image: {
    width: '200px',
    marginLeft: '5px'
  }
}
