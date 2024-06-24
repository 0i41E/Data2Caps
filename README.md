# Data2Caps

----

Data2Caps is a C# project that converts messages or files into a sequence of light intervals of the lock keys of a keyboard. 
This play of lights can be used to exfiltrate those data. This method, also called keystroke reflection, can be used to steal data from isolated systems or to make detection more difficult.
Devices such as Hak5's USB Rubber Ducky Mk II can be used to utilize this form of data exfiltration.

_This method of exfiltration tends to be slow. Exfiltrating large files or messages may take several minutes, up to multiple hours._

References:
  - [Hak5 - Keystroke Reflection](https://shop.hak5.org/pages/keystroke-reflection)
  - [Keystroke Reflection - White Paper](https://cdn.shopify.com/s/files/1/0068/2142/files/hak5-whitepaper-keystroke-reflection.pdf?v=1659317977)
  - [USB Rubber Ducky](https://shop.hak5.org/products/usb-rubber-ducky)
  - [Darren Kitchen](https://x.com/hak5darren)
  - [Dallas Winger](https://x.com/NotKorben)
  - [Example Attack](https://github.com/hak5/usbrubberducky-payloads/tree/master/payloads/library/credentials/BitLockerKeyDump)

----

## Command Line Usage
![Menu](https://github.com/0i41E/Data2Caps/assets/79219148/992f130b-03f7-42fe-9486-11afc8e1bc1e)

## Duckyscript example
This example can be utilized to see the binary in action.
After everything is set up, use the following payload with your Rubber Ducky Mk II.
Wait for the LED to shine in the color red, then execute Data2Caps with the desired message to exfiltrate.
The green led will indicate the success.
Storage will be opened at the end, so the contents of the `loot.bin` file can be checked
```
    DELAY 3000
    SAVE_HOST_KEYBOARD_LOCK_STATE
    $_EXFIL_MODE_ENABLED = TRUE
    $_EXFIL_LEDS_ENABLED = TRUE
    DELAY 500
    LED_R
    WAIT_FOR_SCROLL_CHANGE
    DELAY 250
    LED_G
    DELAY 250
    $_EXFIL_MODE_ENABLED = FALSE
    DELAY 500
    RESTORE_HOST_KEYBOARD_LOCK_STATE
    DELAY 500
    ATTACKMODE STORAGE
```
